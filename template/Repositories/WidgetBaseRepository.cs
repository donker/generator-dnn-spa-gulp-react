using <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Data;
using <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Models.<%= props.widgetName %>s;

namespace <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Repositories
{
    public class <%= props.widgetName %>BaseRepository : RepositoryImpl<<%= props.widgetName %>Base>
    {
    }
}

