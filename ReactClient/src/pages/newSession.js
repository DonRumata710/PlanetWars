import React, { Component } from 'react'
import Page from '../components/page'
import { createSession, getDefaultGameParameters } from '../services/launchService'
import PropertyList from '../components/propertyList'
import LinkButton from '../components/linkButton';

class NewSession extends Component {
    constructor(props) {
        super(props);
        this.isSent = false;
        this.isCreated = false;
    }

    getParameters() {
        var res = {};
        this.parameters.forEach(element => {
            res[element.field] = element.currentValue
        });
        return res;
    }

    applyParameters(parameters) {
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
                currentValue: parameters.planetCount
            },
            {
                field: "PlayerLimit",
                name: "Player limit",
                type: "number",
                currentValue: parameters.playerLimit
            },
            {
                field: "Size",
                name: "Map size",
                type: "number",
                currentValue: parameters.size
            }
        ];
        this.forceUpdate();
    }

    render() {
        if (this.parameters)
        {
            return (
                <Page>
                    <PropertyList properties={this.parameters} />
                    <LinkButton onClick={async () => {
                            this.id = await createSession(this.getParameters())
                            return {
                                pathname: "/session/" + this.id,
                                state: this.parameters
                            }
                        }}>
                            Register room
                    </LinkButton>
                </Page>
            )
        }
        else
        {
            if (!this.isSent)
            {
                getDefaultGameParameters().then((parameters) => {
                    this.applyParameters(parameters)
                }).catch((reason) => {
                    console.log("getDefaultGameParameters failed ", reason)
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
