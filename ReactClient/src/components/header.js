import React from 'react';
import { NavLink } from "react-router-dom";
import logo from '../logo.svg';

function Header() {
    return (
        <div class="App-header">
            <NavLink exact activeClassName="active" to="/"><img style={{width: 50, height: 50}} src={logo} className="App-logo" alt="../logo" /></NavLink>
        </div>
    )
}

export default Header;
