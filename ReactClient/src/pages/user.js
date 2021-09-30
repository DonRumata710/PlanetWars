
import React, { Component } from 'react'
import Page from '../components/page';
import { getUserInfo } from '../services/launchService';

class UserPage extends Component
{
    constructor(props) {
        super(props)
        this.username = props.match.params.userId;
        this.isSent = false;
    }

    render() {
        if (this.info)
        {
            return (
                <Page>
                    <p>Name: {this.info.name}</p>
                    <p>E-mail: {this.info.email}</p>
                    <p>Registration date: {this.info.registerTime}</p>
                </Page>
            );
        }
        else
        {
            if (!this.isSent)
            {
                getUserInfo(this.username).then((value) => {
                    this.info = value;
                    this.forceUpdate();
                });
                this.isSent = true;
            }

            return (
                <Page />
            )
        }
    }
}

export default UserPage;
