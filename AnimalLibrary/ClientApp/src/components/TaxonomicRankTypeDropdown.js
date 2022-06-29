import React, { Component } from 'react';

export class TaxonomicRankTypeDropdown extends Component {
    constructor(props) {
        super(props);    
        debugger;
        this.state = {            
            Value: this.props.value,
            onChange: this.props.onChange,
            TaxonomicRankTypes: []
        };
    }

    onChange(e) {
        this.state.onChange(e);
    }
    
    async populateTaxonomicRankType() {
        const response = await fetch('values/GetAll');
        const data = await response.json();
        this.setState({ TaxonomicRankTypes: data });
    }

    createSelectItems() {
        this.populateTaxonomicRankType();                
        let items = [];        
        for (let i = 0; i < this.state.TaxonomicRankTypes.length; i++) {
            var TaxonomicRankType = this.state.TaxonomicRankTypes[i];
            items.push(<option key={TaxonomicRankType.taxonomicRankTypeID} value={TaxonomicRankType.taxonomicRankTypeID}>{TaxonomicRankType.name}</option>);
        }
        return items;
    }

    render() {
        let items = this.createSelectItems();
        var value = this.state.TaxonomicRankTypes.find(o => o.TaxonomicRankTypeID === this.state.Value);        
        return (<select value={value} onChange={(e) => this.onChange(e)} name={this.props.name}>
            {items}
        </select>);
    }
}