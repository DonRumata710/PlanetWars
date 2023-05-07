import React, { Component } from 'react';
import Page from '../components/page'
import Planet0 from '../resources/game/planet0.png'
import Planet1 from '../resources/game/planet1.png'
import Planet2 from '../resources/game/planet2.png'
import Planet3 from '../resources/game/planet3.png'
import Planet4 from '../resources/game/planet4.png'
import Planet5 from '../resources/game/planet5.png'
import Planet6 from '../resources/game/planet6.png'

class Cell extends Component
{
    constructor(props) {
        super(props)
        this.type = props.type
        console.log("cell ", props)
        if (this.type !== null)
        {
            if (this.type === 0)
                this.LogoImg = Planet0
            else if (this.type === 1)
                this.LogoImg = Planet1
            else if (this.type === 2)
                this.LogoImg = Planet2
            else if (this.type === 3)
                this.LogoImg = Planet3
            else if (this.type === 4)
                this.LogoImg = Planet4
            else if (this.type === 5)
                this.LogoImg = Planet5
            else if (this.type === 6)
                this.LogoImg = Planet6
        }
    }

    render() {
        if (this.type)
        {
            return (
                <button className="cell"><img src={this.LogoImg} alt=""/></button>
            )
        }
        else
        {
            return (<div className="cell"></div>)
        }
    }
}


class Board extends Component
{
    constructor(props) {
        super(props)
        this.size = props.size
        this.cells = Array(this.size).fill(null).map((_, i) => Array(this.size).fill(null).map((_, j) => (<Cell type={1}></Cell>)));
    }

    render() {
        return (
            <div className="board">
                {this.cells.map((row) =>
                    <div className="board_row">
                        {row.map((cell) =>
                            cell
                        )}
                    </div>
                )}
            </div>
        )
    }
}

class GamePage extends Component
{
    render() {
        return (
            <Page>
              <Board size={10}></Board>
            </Page>
        )
    }
}

export default GamePage;
