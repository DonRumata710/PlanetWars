import React, { Component } from 'react';
import Header from './header';
import Content from './content';

class Page extends Component {
    render() {
        return (
            <div>
                <Header />
                <Content>
                    {this.props.children}
                </Content>
            </div>
        )
    }
}

export default Page
