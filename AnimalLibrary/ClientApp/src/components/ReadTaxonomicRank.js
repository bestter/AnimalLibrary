import React, { Component } from 'react';
import { ReadTaxonomicRankSub } from './ReadTaxonomicRankSub';

export class ReadTaxonomicRank extends Component {

    constructor(props) {
        super(props);
        this.state = { TaxonomicRanks: [], loading: true, selectedTR: null, newTr: false, emptyTr: null };        
    }

    async componentDidMount() {        
        await fetch('values/GetAllTaxonomicRankAsync')
            .then(resp => resp.json())
            .then(data => this.setState({ TaxonomicRanks: data }))
        .then(
        fetch('values/GetEmptyTaxonomicRank')
            .then(resp => resp.json())
            .then(data2 => this.setState({ emptyTr: data2, selectedTR: data2, loading: false })));
    }

    edit(TaxonomicRank) {
        console.dir(TaxonomicRank);
        this.setState({ selectedTR: TaxonomicRank });
    }

    resetSelectedTR() {
        this.setState({ selectedTR: null, newTr: false });
        console.log('resetSelectedTR');
    }

    createElement() {
        this.setState({ newTr: true, selectedTR: this.state.emptyTr });
    }

    render() {
        
        return (
            <div>
                <div>
                    <ReadTaxonomicRankSub newTr={ this.state.newTr} emptyTr={this.state.emptyTr} resetSelectedTR={() => this.resetSelectedTR()} selectedTR={this.state.selectedTR} TaxonomicRanks={this.state.TaxonomicRanks} edit={(item) => this.edit(item) }/>
                </div>
                <button type="button" className="btn btn-success" onClick={() => this.createElement()}>New</button>
            </div>
        );
    }

}
