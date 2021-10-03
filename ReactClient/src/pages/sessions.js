import React, { Component } from 'react'
import { NavLink } from 'react-router-dom';
import ClaimPanel from '../components/claimPanel';
import Page from '../components/page'
import { getSessions } from '../services/launchService'

function Session(props) {
    const name = props.name;
    const description = props.description;
    return (
        <ClaimPanel>
            <NavLink to={"/session/" + props.id}>
                <h3>{name}</h3>
                {description}
            </NavLink>
        </ClaimPanel>
    );
}


class Sessions extends Component {
    constructor(props) {
        super(props);
        this.isSent = false;
    }

    render() {
        if (this.sessionList)
        {
            return (
                <Page>
                    <h2>Pending sessions:</h2>
                    {this.sessionList}
                </Page>
            )
        }
        else
        {
            if (!this.isSent)
            {
                getSessions().then((sessions) => {
                    this.sessionList = Object.keys(sessions).map(key =>
                        <Session key={key} id={key} name={sessions[key].parameters.name} description={sessions[key].parameters.description} />
                    );
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

export default Sessions
