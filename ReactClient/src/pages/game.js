import React, { Component } from 'react';
import Page from '../components/page'

class Cell extends Component
{
    render() {
        return (
            <button className="cell"></button>
        )
    }
}


class Board extends Component
{
    constructor(props) {
        super(props)
        this.size = props.size
    }

    renderCell(i) {
        return (
            <Cell></Cell>
        )
    }

    render() {
        var rows = new Array(this.size)
        for (var i = 0; i < this.size; ++i)
            rows[i] = this.renderCell()

        return (
            <div>
                <div className="board-row">

                </div>
            </div>
        )
    }
}

class GamePage extends Component
{
    render() {
        return (
            <Page>
              <Board size='10'></Board>
            </Page>
        )
    }
}

export default GamePage;
