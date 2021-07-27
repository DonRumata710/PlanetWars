import React from 'react'
import { NavLink } from 'react-router-dom';
import Page from '../components/page'
import { getSessions } from '../services/launchService'

function Session(props) {
    const name = props.name;
    const description = props.description;
    return (
        <div className="session">
            <NavLink to="/game">
                <h1>{name}</h1>
                {description}
            </NavLink>
        </div>
    );
}

function Sessions() {
    const sessionList = getSessions();

    sessionList.map((session) =>
        <Session>{session}</Session>
    );

    return (
        <Page>
            <Sessions>{sessionList}</Sessions>
        </Page>
    )
}

export default Sessions
