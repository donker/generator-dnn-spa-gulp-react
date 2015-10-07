namespace <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Common
{
    public static class Globals
    {

        public const string ClientResourceFileName = "~/DesktopModules/<%= props.organization %>/<%= props.projectName %>/App_LocalResources/ClientResources.resx";
        public const string SharedResourceFileName = "~/DesktopModules/<%= props.organization %>/<%= props.projectName %>/App_LocalResources/SharedResources.resx";

    }
}