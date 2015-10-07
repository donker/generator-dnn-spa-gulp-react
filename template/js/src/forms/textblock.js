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
      value: e.target.value
    });
  },

  getValue: function() {
    return this.refs.txtInput.getDOMNode().value;
  },

  render: function() {
    return (
      <div className={this.props.groupClass} ref="mainDiv">
       <label htmlFor={this.props.text}>
        <span>{this.props.text}</span>
       </label>
       <textarea ref="txtInput" value={this.state.value} onChange={this.handleChange} className={this.props.className} />
      </div>
    );
  },

});

module.exports = Input;
