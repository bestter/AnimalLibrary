import React, { Component } from 'react';
import { UpdateTaxonomicRank } from './UpdateTaxonomicRank';

export class ReadTaxonomicRankSub extends Component {
    render() {        
        if (this.props.newTr === true) {
            return <UpdateTaxonomicRank TaxonomicRank={this.props.emptyTr} resetSelectedTR={() => this.props.resetSelectedTR()} newElement={true} />;
        }
        else if (this.props.selectedTR && this.props.selectedTR.taxonomicRankID > 0) {
            return <UpdateTaxonomicRank TaxonomicRank={this.props.selectedTR} resetSelectedTR={() => this.props.resetSelectedTR()} newElement={false} />;
        }
        else if (this.props.selectedTR == null || this.props.selectedTR === this.props.emptyTr) {            
            let taxonomicRanks = this.props.TaxonomicRanks;
            return (
                <div>
                    <ul className="list-group">
                        {
                            taxonomicRanks.map((item, key) => {
                                return (<li className="list-group-item" key={item.taxonomicRankID}> {item.name}<button onClick={() => { this.props.edit(item); }} className="btn btn-primary bi bi-pencil">edit</button></li>);
                            })
                        }
                    </ul>
                </div>
            );
        }
        else {
            return (<span>vide</span>);
        }
    }
}