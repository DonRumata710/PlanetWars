import React, { Component } from 'react'
import Page from '../components/page'
import { createSession, getDefaultGameParameters } from '../services/launchService'
import PropertyList from '../components/propertyList'
import LinkButton from '../components/linkButton';

class NewSession extends Component {
    constructor(props) {
        super(props);
        console.log(props)
        this.isSent = false;
        this.isCreated = false;
        this.id = props.match.params.id;
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
                    <LinkButton to={"/session/" + this.id} onClick={() => {
                            createSession(this.getParameters()).then((value) => {
                                this.id = value;
                                this.forceUpdate();
                            });
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
