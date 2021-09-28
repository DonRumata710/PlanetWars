import React, { Component } from 'react'
import Page from '../components/page'
import { getDefaultGameParameters } from '../services/launchService'
import PropertyList from '../components/propertyList'

class NewSession extends Component {
    render() {
        if (this.defaultData)
        {
            var parameters = [
                {
                    name: "Planet count",
                    type: "number",
                    currentValue: this.defaultData.planetCount
                },
                {
                    name: "Player limit",
                    type: "number",
                    currentValue: this.defaultData.playerLimit
                },
                {
                    name: "Map size",
                    type: "number",
                    currentValue: this.defaultData.size
                }
            ]

            return (
                <Page>
                    <PropertyList properties={parameters} />
                </Page>
            )
        }
        else
        {
            getDefaultGameParameters().then((value) => {
                this.defaultData = value;
                this.forceUpdate();
            });

            return (
                <Page />
            )
        }
    }
}

export default NewSession
