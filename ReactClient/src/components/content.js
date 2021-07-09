import React, { Component } from 'react';

class Content extends Component {
    render() {
        return (
            <div class="App-content">
                {this.props.children}
            </div>
        )
    }
}

export default Content;
