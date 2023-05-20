import React, { Component } from 'react';

class PropertyList extends Component {
    constructor(props) {
        super(props);

        this.propertyFields = this.props.properties.map((prop) => {
            return (<div key={prop.name}>{prop.name}:
                <input
                    type={prop.type}
                    defaultValue={prop.currentValue}
                    onChange={(input) => { prop.currentValue = input.target.value; }}
                    readOnly={this.props.editPermission === false}>
                </input></div>)
        });
    }

    render() {
        return (
            <div className="property-list">
                {this.propertyFields}
            </div>
        )
    }
}

export default PropertyList;
