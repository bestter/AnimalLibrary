import React, { Component } from 'react';
import { UpdateTaxonomicRank } from './UpdateTaxonomicRank';

export class ReadTaxonomicRank extends Component {

    constructor(props) {
        super(props);
        this.state = { TaxonomicRanks: [], loading: true, selectedTR: null, newTr: false, emptyTr: null };        
    }

    async componentDidMount() {
        console.log('ReadTaxonomicRank componentDidMount');
        await fetch('values/GetAllTaxonomicRankAsync')
            .then(resp => resp.json())
            .then(data => this.setState({ TaxonomicRanks: data }));
        await fetch('values/GetEmptyTaxonomicRank')
            .then(resp => resp.json())
            .then(data2 => this.setState({ emptyTr: data2, selectedTR: data2, loading: false }));
    }

    edit(TaxonomicRank) {
        this.setState({ selectedTR: TaxonomicRank });
    }

    resetSelectedTR() {
        this.setState({ selectedTR: null, newTr: false });
        console.log('resetSelectedTR');
    }

    createElement() {
        this.setState({ newTr: true, selectedTR: this.state.emptyTr });
    }

    getCode() {
        debugger;
        if (this.state.newTr === true) {
            return <UpdateTaxonomicRank TaxonomicRank={this.state.emptyTr} resetSelectedTR={() => this.resetSelectedTR()} newElement={true} />;
        }
        else if (this.state.selectedTR && this.state.selectedTR.taxonomicRankID > 0) {
            return <UpdateTaxonomicRank TaxonomicRank={this.state.selectedTR} resetSelectedTR={() => this.resetSelectedTR()} newElement={false} />;
        }
        else if (this.state.selectedTR == null) {            
            let taxonomicRanks = this.state.TaxonomicRanks;
            return (
                <div>
                    <ul className="list-group">
                        {
                            taxonomicRanks.map((item, key) => {
                                return (<li className="list-group-item" key={item.taxonomicRankID}> {item.name}<button onClick={() => { this.edit(item); }} className="btn btn-primary bi bi-pencil">edit</button></li>);
                            })
                        }
                    </ul>
                </div>
            );
        }
    }

    render() {
        const code = this.getCode();

        return (
            <div>
                <div>
                    {code}
                </div>
                <button type="button" className="btn btn-success" onClick={() => this.createElement()}>New</button>
            </div>
        );
    }

}
