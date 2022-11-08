import React, { Component } from 'react';

export class SpeciesList extends Component {

    constructor(props) {
        super(props);        
    }


    getSpecies() {
        let items = [];
        for (let i = 0; i < this.props.Species.length; i++) {
            var Specie = this.props.Species[i];            
            items.push(<option key={Specie.id} id={Specie.id} value={Specie.id}>{Specie.name}</option>);
        }
        return items;

    }
    render()
    {
        if (this.props.Species != null ) {
            var speciesItems = this.getSpecies();
            return (
                <div>
                    <ul className="list-group">
                        {speciesItems}
                    </ul>
                </div>
            );
        }
        else {
            return (<span>vide</span>);
        }
    }
}