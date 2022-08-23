import React, { Component } from 'react';

export class TaxonomicRankTypeDropdown extends Component {
    constructor(props) {
        super(props);
        this.state = {            
            Value: this.props.value,
            onChange: this.props.onChange,
            TaxonomicRankTypes: []
        };
        this.populateTaxonomicRankType();                
    }

    onChange(e) {
        this.state.onChange(e);
    }
    
    async populateTaxonomicRankType() {
        const response = await fetch('values/GetAllTaxonomicRankTypeAsync');
        const data = await response.json();
        this.setState({ TaxonomicRankTypes: data });
    }

    createSelectItems() {        
        let items = [];        
        for (let i = 0; i < this.state.TaxonomicRankTypes.length; i++) {            
            var TaxonomicRankType = this.state.TaxonomicRankTypes[i];            
            items.push(<option key={TaxonomicRankType.taxonomicRankTypeID} value={TaxonomicRankType.taxonomicRankTypeID} >{TaxonomicRankType.name}</option>);
        }
        return items;
    }

    render() {        
        let items = this.createSelectItems();        
        return (<select value={this.state.Value} onChange={(e) => this.onChange(e)} name={this.props.name} key={this.props.name}>
            {items}
        </select>);
    }
}