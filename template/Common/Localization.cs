using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Xml.XPath;
using DotNetNuke.Collections.Internal;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.Cache;
using DotNetNuke.Services.Localization;

namespace <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Common
{
    public class Localization
    {

        public static Dictionary<string, string> GetResourceFile(PortalSettings portalSettings, string resourceFile, string locale)
        {
            return
                (Dictionary<string, string>) GetCompiledResourceFileCallBack(
                    new CacheItemArgs("Compiled-" + resourceFile + "-" + locale + "-" + portalSettings.PortalId,
                        DataCache.ResourceFilesCacheTimeOut, DataCache.ResourceFilesCachePriority, resourceFile, locale,
                        portalSettings));
        }

        private static object GetCompiledResourceFileCallBack(CacheItemArgs cacheItemArgs)
        {
            string resourceFile = (string)cacheItemArgs.Params[0];
            string locale = (string)cacheItemArgs.Params[1];
            PortalSettings portalSettings = (PortalSettings)cacheItemArgs.Params[2];
            string systemLanguage = DotNetNuke.Services.Localization.Localization.SystemLocale;
            string defaultLanguage = portalSettings.DefaultLanguage;
            string fallbackLanguage = DotNetNuke.Services.Localization.Localization.SystemLocale;
            Locale targetLocale = LocaleController.Instance.GetLocale(locale);
            if (!String.IsNullOrEmpty(targetLocale.Fallback))
            {
                fallbackLanguage = targetLocale.Fallback;
            }

            // get system default and merge the specific ones one by one
            var res = GetResourceFile(resourceFile);
            if (res == null)
            {
                return new Dictionary<string, string>();
            }
            res = MergeResourceFile(res, GetResourceFileName(resourceFile, systemLanguage, portalSettings.PortalId));
            if (defaultLanguage != systemLanguage)
            {
                res = MergeResourceFile(res, GetResourceFileName(resourceFile, defaultLanguage, -1));
                res = MergeResourceFile(res, GetResourceFileName(resourceFile, defaultLanguage, portalSettings.PortalId));
            }
            if (fallbackLanguage != defaultLanguage)
            {
                res = MergeResourceFile(res, GetResourceFileName(resourceFile, fallbackLanguage, -1));
                res = MergeResourceFile(res, GetResourceFileName(resourceFile, fallbackLanguage, portalSettings.PortalId));
            }
            if (locale != fallbackLanguage)
            {
                res = MergeResourceFile(res, GetResourceFileName(resourceFile, locale, -1));
                res = MergeResourceFile(res, GetResourceFileName(resourceFile, locale, portalSettings.PortalId));
            }
            return res;
        }

        private static Dictionary<string, string> MergeResourceFile(Dictionary<string, string> current, string resourceFile)
        {
            var resFile = GetResourceFile(resourceFile);
            if (resFile == null)
            {
                return current;
            }
            foreach (string key in current.Keys.ToList())
            {
                if (resFile.ContainsKey(key))
                {
                    current[key] = resFile[key];
                }
            }
            return current;
        }

        #region Core Localization
        // Copy of methods from core localization provider but which kept these methods private, making it impossible to get a resource file from the cache
        public static Dictionary<string, string> GetResourceFile(string resourceFile)
        {
            return CBO.GetCachedObject<Dictionary<string, string>>(new CacheItemArgs(resourceFile, DataCache.ResourceFilesCacheTimeOut, DataCache.ResourceFilesCachePriority),
                                                                   GetResourceFileCallBack,
                                                                   true);
        }

        private static object GetResourceFileCallBack(CacheItemArgs cacheItemArgs)
        {
            string cacheKey = cacheItemArgs.CacheKey;
            Dictionary<string, string> resources = null;

            string filePath = null;
            try
            {
                //Get resource file lookup to determine if the resource file even exists
                SharedDictionary<string, bool> resourceFileExistsLookup = GetResourceFileLookupDictionary();

                if (ResourceFileMayExist(resourceFileExistsLookup, cacheKey))
                {
                    //check if an absolute reference for the resource file was used
                    if (cacheKey.Contains(":\\") && Path.IsPathRooted(cacheKey))
                    {
                        //if an absolute reference, check that the file exists
                        if (File.Exists(cacheKey))
                        {
                            filePath = cacheKey;
                        }
                    }

                    //no filepath found from an absolute reference, try and map the path to get the file path
                    if (filePath == null)
                    {
                        filePath = HostingEnvironment.MapPath(DotNetNuke.Common.Globals.ApplicationPath + cacheKey);
                    }

                    //The file is not in the lookup, or we know it exists as we have found it before
                    if (File.Exists(filePath))
                    {
                        if (filePath != null)
                        {
                            var doc = new XPathDocument(filePath);
                            resources = new Dictionary<string, string>();
                            foreach (XPathNavigator nav in doc.CreateNavigator().Select("root/data"))
                            {
                                if (nav.NodeType != XPathNodeType.Comment)
                                {
                                    var selectSingleNode = nav.SelectSingleNode("value");
                                    if (selectSingleNode != null)
                                    {
                                        resources[nav.GetAttribute("name", String.Empty)] = selectSingleNode.Value;
                                    }
                                }
                            }
                        }
                        cacheItemArgs.CacheDependency = new DNNCacheDependency(filePath);

                        //File exists so add it to lookup with value true, so we are safe to try again
                        using (resourceFileExistsLookup.GetWriteLock())
                        {
                            resourceFileExistsLookup[cacheKey] = true;
                        }
                    }
                    else
                    {
                        //File does not exist so add it to lookup with value false, so we don't try again
                        using (resourceFileExistsLookup.GetWriteLock())
                        {
                            resourceFileExistsLookup[cacheKey] = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("The following resource file caused an error while reading: {0}", filePath), ex);
            }
            return resources;
        }

        private static SharedDictionary<string, bool> GetResourceFileLookupDictionary()
        {
            return
                CBO.GetCachedObject<SharedDictionary<string, bool>>(
                    new CacheItemArgs(DataCache.ResourceFileLookupDictionaryCacheKey, DataCache.ResourceFileLookupDictionaryTimeOut, DataCache.ResourceFileLookupDictionaryCachePriority),
                    c => new SharedDictionary<string, bool>(),
                    true);
        }

        private static bool ResourceFileMayExist(SharedDictionary<string, bool> resourceFileExistsLookup, string cacheKey)
        {
            bool mayExist;
            using (resourceFileExistsLookup.GetReadLock())
            {
                mayExist = !resourceFileExistsLookup.ContainsKey(cacheKey) || resourceFileExistsLookup[cacheKey];
            }
            return mayExist;
        }

        private static string GetResourceFileName(string resourceFileRoot, string language, int portalId)
        {
            string resourceFile;
            language = language.ToLower();
            if (resourceFileRoot != null)
            {
                if (language == DotNetNuke.Services.Localization.Localization.SystemLocale.ToLower() || String.IsNullOrEmpty(language))
                {
                    switch (resourceFileRoot.Substring(resourceFileRoot.Length - 5, 5).ToLower())
                    {
                        case ".resx":
                            resourceFile = resourceFileRoot;
                            break;
                        case ".ascx":
                            resourceFile = resourceFileRoot + ".resx";
                            break;
                        case ".aspx":
                            resourceFile = resourceFileRoot + ".resx";
                            break;
                        default:
                            resourceFile = resourceFileRoot + ".ascx.resx"; //a portal module
                            break;
                    }
                }
                else
                {
                    switch (resourceFileRoot.Substring(resourceFileRoot.Length - 5, 5).ToLower())
                    {
                        case ".resx":
                            resourceFile = resourceFileRoot.Replace(".resx", "." + language + ".resx");
                            break;
                        case ".ascx":
                            resourceFile = resourceFileRoot.Replace(".ascx", ".ascx." + language + ".resx");
                            break;
                        case ".aspx":
                            resourceFile = resourceFileRoot.Replace(".aspx", ".aspx." + language + ".resx");
                            break;
                        default:
                            resourceFile = resourceFileRoot + ".ascx." + language + ".resx";
                            break;
                    }
                }
            }
            else
            {
                if (language == DotNetNuke.Services.Localization.Localization.SystemLocale.ToLower() || String.IsNullOrEmpty(language))
                {
                    resourceFile = DotNetNuke.Services.Localization.Localization.SharedResourceFile;
                }
                else
                {
                    resourceFile = DotNetNuke.Services.Localization.Localization.SharedResourceFile.Replace(".resx", "." + language + ".resx");
                }
            }
            if (portalId != -1)
            {
                resourceFile = resourceFile.Replace(".resx", ".Portal-" + portalId + ".resx");
            }
            return resourceFile;
        }
        #endregion

    }
}