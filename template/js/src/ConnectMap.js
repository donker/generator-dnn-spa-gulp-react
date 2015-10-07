/** @jsx React.DOM */
var ConnectMapSettings = require('./ConnectMapSettings'),
  EditMapPoint = require('./EditMapPoint'),
  Icon = require('./forms/icons'),
  MapPointMessage = require('./MapPointMessage');

var ConnectMapComponent = React.createClass({

  _map: {},
  _mapListener: {},

  onSettingsUpdate: function(newSettings) {
    var that = this;
    this.state.service.updateSettings(newSettings, function(data) {
      that.setState({
        settings: data
      });
      that.setMapSize();
    });
  },

  setMap: function() {
    if (confirm(this.state.resources.SetMapConfirm)) {
      var newSettings = this.state.settings;
      newSettings.MapOriginLat = this._map.getCenter().lat();
      newSettings.MapOriginLong = this._map.getCenter().lng();
      newSettings.Zoom = this._map.getZoom();
      this.state.service.updateSettings(newSettings);
    }
    return false;
  },

  setMapSize: function() {
    var mapDiv = $(this.refs.mapDiv.getDOMNode());
    mapDiv.width(this.state.settings.MapWidth);
    mapDiv.height(this.state.settings.MapHeight);
  },

  getInitialState: function() {
    return {
      moduleId: this.props.moduleId,
      service: ConnectMap.modules[this.props.moduleId].service,
      security: ConnectMap.modules[this.props.moduleId].security,
      settings: ConnectMap.modules[this.props.moduleId].settings,
      mapPoints: ConnectMap.modules[this.props.moduleId].mapPoints,
      resources: ConnectMap.modules[this.props.moduleId].resources,
      isAdding: false
    }
  },

  addPoint: function() {
    if (this.state.isAdding) {
      return this.stopAddingPoint();
    }
    this._map.setOptions({
      draggableCursor: 'crosshair'
    });
    this.setState({
      isAdding: true
    });
    var that = this;
    this._mapListener = google.maps.event.addListener(this._map, 'click', function(e) {
      var newPoint = {
        Latitude: e.latLng.lat(),
        Longitude: e.latLng.lng(),
        Message: '',
        MapPointId: -1
      };
      React.render(
        <EditMapPoint MapPoint={newPoint} onUpdate={that.onAddPoint} resources={that.state.resources} />, $('#connectMapPanel')[0]);
      window.ConnectMap.slidePanel($('#connectMapPanel'));
      that.stopAddingPoint();
    });
    return false;
  },

  stopAddingPoint: function() {
    this._map.setOptions({
      draggableCursor: 'grab'
    });
    this.setState({
      isAdding: false
    });
    var that = this;
    google.maps.event.removeListener(this._mapListener);
    return false;
  },

  onAddPoint: function(newMapPoint, marker) {
    var that = this;
    this.state.service.submitPoint(newMapPoint, function(data) {
      if (marker === undefined) {
        that.addPointToMap(data);
        var newPoints = that.state.mapPoints;
        newPoints.push(data);
        that.setState({
          mapPoints: newPoints
        });
      } else {
        var newPoints = that.state.mapPoints;
        for (var i = 0; i < newPoints.length; i++) {
          if (newPoints[i].MapPointId === newMapPoint.MapPointId) {
            newPoints[i] = newMapPoint;
          }
        }
        that.setState({
          mapPoints: newPoints
        });
      }
    });
  },

  onDeletePoint: function(mapPoint, marker) {
    this.state.service.deletePoint(mapPoint.MapPointId, function(data) {
      marker.setMap(null);
    });
  },

  addPointToMap: function(point) {
    var canEdit = ((this.state.security.IsPointer && (point.CreatedByUserID == this.state.security.UserId || this.state.settings.AllowOtherEdit)) || this.state.security.CanEdit);
    var marker = new google.maps.Marker({
      position: new google.maps.LatLng(point.Latitude, point.Longitude),
      map: this._map,
      draggable: canEdit,
      mapPoint: point
    });
    var msg = $('<div id="point' + point.MapPointId + '" class="conPointMessage"></div>').appendTo('body');
    React.render(
      <MapPointMessage MapPoint={point} CanEdit={canEdit} OnEdit={this.onAddPoint} OnDelete={this.onDeletePoint} Marker={marker} resources={this.state.resources} />,
      msg[0]);
    var infowindow = new google.maps.InfoWindow();
    infowindow.setContent(msg[0]);
    msg.remove();
    var that = this;
    google.maps.event.addListener(marker, 'click', function(e) {
      infowindow.open(that._map, marker);
    });
    google.maps.event.addListener(marker, 'dragend', function(e) {
      var changedPoint = marker.mapPoint;
      changedPoint.Latitude = e.latLng.lat();
      changedPoint.Longitude = e.latLng.lng();
      that.onAddPoint(changedPoint, marker);
    });
  },

  shouldComponentUpdate: function(nextProps, nextState) {
    return nextState.settings !== this.state.settings;
  },

  componentDidMount: function() {
    this.setMapSize();
    this._map = new google.maps.Map(this.refs.mapDiv.getDOMNode(), {
      center: new google.maps.LatLng(this.state.settings.MapOriginLat, this.state.settings.MapOriginLong),
      zoom: this.state.settings.Zoom,
      mapTypeId: google.maps.MapTypeId.ROADMAP
    });
    var that = this;
    $.each(this.state.mapPoints, function(index, item) {
      that.addPointToMap(item);
    });
    $('.connectMapSettings').off("click");
    $('.connectMapSettings').click(function() {
      React.render(
        <ConnectMapSettings Settings={that.state.settings} onUpdate={that.onSettingsUpdate} resources={that.state.resources} />, $('#connectMapPanel')[0]);
      window.ConnectMap.slidePanel($('#connectMapPanel'));
      return false;
    });
  },

  render: function() {

    var editSettingsLink = (<span />);
    var setMapLink = (<span />);
    var addPointLink = (<span />);
    if (this.state.security.CanEdit) {
      editSettingsLink = (
        <a href="#" className="conLink connectMapSettings" title={this.state.resources.ShowSettings}>
         <Icon type="map" />
        </a>
      );
      setMapLink = (
        <a href="#" className="conLink" onClick={this.setMap} title={this.state.resources.SetMap}>
         <Icon type="crosshairs" />
        </a>
      );
    }
    if (this.state.security.CanEdit) {
      addPointLink = (
        <a href="#" className="conLink" onClick={this.addPoint} title={this.state.resources.AddPoint}>
         <Icon type="map-marker" />
        </a>
      );
    }

    return (
      <div>
        <div ref="mapDiv" />
        <div className="conMgtPanel">
         {editSettingsLink}
         {setMapLink}
         {addPointLink}
        </div>
      </div>
    );
  }

});

module.exports = ConnectMapComponent;
