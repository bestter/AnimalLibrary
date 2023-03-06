import React, { Component } from 'react';

export class TaxonomicRankDropdown extends Component {
    constructor(props) {
        super(props);
        this.state = {
            Value: this.props.value,
            onChange: this.props.onChange,
            TaxonomicRanks: []            
        };
        this.populateTaxonomicRank();
    }

    async populateTaxonomicRank() {
        const response = await fetch('values/GetAllTaxonomicRankAsync');
        const data = await response.json(); 
        this.setState({ TaxonomicRanks: data });
    }


    createSelectItems() {        
        
        let items = [];        
        for (let i = 0; i < this.state.TaxonomicRanks.length; i++) {            
            var TaxonomicRank = this.state.TaxonomicRanks[i];
            items.push(<option key={TaxonomicRank.taxonomicRankID} value={TaxonomicRank.taxonomicRankID}>{TaxonomicRank.name}</option>);
        }
        return items;
    }

    render() {        
        let items = this.createSelectItems();
        return (<select value={this.state.Value ?? -1} onChange={(e) => this.state.onChange(e)} name={this.props.name}>
            {items}
        </select>);
    }
}