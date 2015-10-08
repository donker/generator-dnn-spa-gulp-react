/** @jsx React.DOM */
var <%= props.widgetName %>Component = React.createClass({

  getInitialState: function() {
    return {
      moduleId: this.props.moduleId,
      service: <%= props.organization %><%= props.projectName %>.modules[this.props.moduleId].service,
      security: <%= props.organization %><%= props.projectName %>.modules[this.props.moduleId].security,
      settings: <%= props.organization %><%= props.projectName %>.modules[this.props.moduleId].settings,
      resources: <%= props.organization %><%= props.projectName %>.modules[this.props.moduleId].resources
    }
  },

  shouldComponentUpdate: function(nextProps, nextState) {
    return nextState.settings !== this.state.settings;
  },

  render: function() {
    return (
      <div ref="myDiv">
       Hello World
      </div>
    );
  }

});

module.exports = <%= props.organization %><%= props.projectName %>Component;
