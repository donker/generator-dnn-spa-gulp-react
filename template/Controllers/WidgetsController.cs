using System;
using System.Collections.Generic;
using <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Models.<%= props.widgetName %>s;
using <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Repositories;

namespace <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Controllers
{

    public partial class <%= props.widgetName %>sController
    {

        public static IEnumerable<<%= props.widgetName %>> Get<%= props.widgetName %>s(int moduleId)
        {
            <%= props.widgetName %>Repository repo = new <%= props.widgetName %>Repository();
            return repo.Get(moduleId);
        }

        public static <%= props.widgetName %> Get<%= props.widgetName %>(int <%= props.widgetName %>Id, int moduleId)
        {
            <%= props.widgetName %>Repository repo = new <%= props.widgetName %>Repository();
            return repo.GetById(<%= props.widgetName %>Id, moduleId);
        }

        public static int Add<%= props.widgetName %>(ref <%= props.widgetName %>Base <%= props.widgetName %>, int userId)
        {
            <%= props.widgetName %>.CreatedByUserID = userId;
            <%= props.widgetName %>.CreatedOnDate = DateTime.Now;
            <%= props.widgetName %>.LastModifiedByUserID = userId;
            <%= props.widgetName %>.LastModifiedOnDate = DateTime.Now;
            <%= props.widgetName %>BaseRepository repo = new <%= props.widgetName %>BaseRepository();
            repo.Insert(<%= props.widgetName %>);
            return <%= props.widgetName %>.<%= props.widgetName %>Id;
        }

        public static void Update<%= props.widgetName %>(<%= props.widgetName %>Base <%= props.widgetName %>, int userId)
        {
            <%= props.widgetName %>.LastModifiedByUserID = userId;
            <%= props.widgetName %>.LastModifiedOnDate = DateTime.Now;
            <%= props.widgetName %>BaseRepository repo = new <%= props.widgetName %>BaseRepository();
            repo.Update(<%= props.widgetName %>);
        }

        public static void Delete<%= props.widgetName %>(<%= props.widgetName %>Base <%= props.widgetName %>)
        {
            <%= props.widgetName %>BaseRepository repo = new <%= props.widgetName %>BaseRepository();
            repo.Delete(<%= props.widgetName %>);
        }

    }
}
