import React from 'react';
import { NavLink } from "react-router-dom";
import { login, signOut } from '../services/userService'
import { useSelector } from 'react-redux'
import logo from '../logo.svg';

function Header() {
    const user = useSelector(state => state.auth.user)

    return (
        <div className="app-header">
            <NavLink exact activeClassName="active" to="/"><img style={{width: 50, height: 50}} src={logo} className="app-logo" alt="../logo" /></NavLink>
            <button className="header-widget" onClick={() => user ? signOut() : login()}>{user ? "Logout" : "Login"}</button>
        </div>
    )
}

export default Header;
