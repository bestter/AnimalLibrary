import React, { Component } from 'react';

export class TaxonomicRankDropdown extends Component {
    constructor(props) {
        super(props);
        this.state = {
            Value: this.props.value,
            onChange: this.props.onChange,
            TaxonomicRanks: []            
        };
    }

    
    async populateTaxonomicRank() {
        const response = await fetch('values/GetAllTaxonomicRank');
        const data = await response.json(); 
        this.setState({ TaxonomicRanks: data });
    }

    //onChange(e) {
    //    debugger;
    //    this.state.onchange(e);
    //}

    createSelectItems() {
        this.populateTaxonomicRank();
        
        let items = [];        
        for (let i = 0; i < this.state.TaxonomicRanks.length; i++) {
            var TaxonomicRank = this.state.TaxonomicRanks[i];
            items.push(<option key={TaxonomicRank.TaxonomicRankID} value={TaxonomicRank.TaxonomicRankID}>{TaxonomicRank.name}</option>);
        }
        return items;
    }

    render() {        
        let items = this.createSelectItems();
        return (<select value={this.state.Value ?? -1} onChange={(e) => this.state.onChange(e)} name={ this.props.name }>
            {items}
        </select>);
    }
}