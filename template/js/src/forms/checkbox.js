/** @jsx React.DOM */
var Input = React.createClass({

  getInitialState: function() {
    return {
      value: this.props.value
    }
  },

  componentWillReceiveProps: function(nextProps) {
    this.setState({
      value: nextProps.value
    });
  },

  handleChange: function(e) {
    this.setState({
      value: !this.state.value
    });
  },

  getValue: function() {
    return this.state.value;
  },

  render: function() {
    return (
      <div className={this.props.groupClass} ref="mainDiv">
       <label htmlFor={this.props.text} className="conInputCheck">
        <span>{this.props.text}</span>
       </label>
       <input type="checkbox" ref="chk" checked={this.state.value} onChange={this.handleChange} />
      </div>
    );
  },

});

module.exports = Input;
