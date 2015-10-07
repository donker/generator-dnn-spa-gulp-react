/** @jsx React.DOM */
var EditMapPoint = require('./EditMapPoint');

var MapPointMessage = React.createClass({

  getInitialState: function() {
    return {
      mapPoint: this.props.MapPoint
    }
  },

  componentWillReceiveProps: function(nextProps) {
    this.setState({
      mapPoint: nextProps.MapPoint
    });
  },

  onEdit: function(newMapPoint, marker) {
    this.setState({
      mapPoint: newMapPoint
    });
    this.props.OnEdit(newMapPoint, marker);
  },

  edit: function() {
    React.render(
      <EditMapPoint MapPoint={this.props.MapPoint} onUpdate={this.onEdit} Marker={this.props.Marker} />, $('#connectMapPanel')[0]);
    window.ConnectMap.slidePanel($('#connectMapPanel'));
    return false;
  },

  deletePoint: function() {
    if (confirm(this.props.resources.DeletePointConfirm)) {
      this.props.OnDelete(this.props.MapPoint, this.props.Marker);
    }
  },

  render: function() {

    var editLink;
    var deleteLink;
    if (this.props.CanEdit) {
      editLink = (
        <a href="#" onClick={this.edit}>{this.props.resources.Edit}</a>
        );
      deleteLink = (
        <a href="#" onClick={this.deletePoint}>{this.props.resources.Delete}</a>
        );
    }

    return (
      <div>
       {this.state.mapPoint.Message}<br />
       <em>{this.props.resources.CreatedBy} {this.state.mapPoint.CreatedByUser}</em><br />
       {editLink} {deleteLink}
      </div>
      )
  }

});

module.exports = MapPointMessage;
