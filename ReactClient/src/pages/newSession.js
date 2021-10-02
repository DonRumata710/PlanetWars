import React, { Component } from 'react'
import Page from '../components/page'
import { createSession, getDefaultGameParameters, leaveSession, startSession } from '../services/launchService'
import PropertyList from '../components/propertyList'
import LinkButton from '../components/linkButton';

class NewSession extends Component {
    constructor(props) {
        super(props);
        this.isSent = false;
        this.isCreated = false;
    }

    componentWillUnmount() {
        leaveSession();
    }

    getParameters() {
        var res = {};
        this.parameters.forEach(element => {
            res[element.field] = element.currentValue
        });
        return res;
    }

    render() {
        if (this.parameters)
        {
            return (
                <Page>
                    <PropertyList properties={this.parameters} />
                    <button onClick={() => {
                        createSession(this.getParameters()).then((value) => {
                            this.id = value;
                            this.forceUpdate();
                        });
                    }}>Register room</button>
                    { this.id != null &&
                        <LinkButton to={"/game/" + this.id} onClick={() => {
                            startSession(this.id)
                        }}>Start game</LinkButton>
                    }
                </Page>
            )
        }
        else
        {
            if (!this.isSent)
            {
                getDefaultGameParameters().then((value) => {
                    this.parameters = [
                        {
                            field: "Name",
                            name: "Name",
                            type: "text",
                            currentValue: ""
                        },
                        {
                            field: "Description",
                            name: "Description",
                            type: "text",
                            currentValue: ""
                        },
                        {
                            field: "PlanetCount",
                            name: "Planet count",
                            type: "number",
                            currentValue: value.planetCount
                        },
                        {
                            field: "PlayerLimit",
                            name: "Player limit",
                            type: "number",
                            currentValue: value.playerLimit
                        },
                        {
                            field: "Size",
                            name: "Map size",
                            type: "number",
                            currentValue: value.size
                        }
                    ];
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

export default NewSession
