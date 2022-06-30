import React, { Component } from 'react';
import { TaxonomicRankTypeDropdown } from './TaxonomicRankTypeDropdown';
import { TaxonomicRankDropdown } from './TaxonomicRankDropdown';

export class UpdateTaxonomicRank extends Component {

    constructor(props) {
        super(props);
        this.state = {
            taxonomicRankID: this.props.TaxonomicRank.taxonomicRankID,
            parentTaxonomicRankID: this.props.TaxonomicRank.parentTaxonomicRankID,
            name: this.props.TaxonomicRank.name,
            loading: true,
            taxonomicRankTypeId: this.props.TaxonomicRank.taxonomicRankTypeId,
            resetSelectedTR: this.props.resetSelectedTR,
            newElement: this.props.newElement
        };
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    async handleSubmit(event) {
        event.preventDefault();
        const data = { TaxonomicRankID: this.state.taxonomicRankID, ParentTaxonomicRankID: this.state.parentTaxonomicRankID, name: this.state.name, TaxonomicRankTypeId: this.state.taxonomicRankTypeId };
        var response;
        if (this.state.newElement === false) {
            response = await fetch("values/UpdateTaxonomicRankAsync", {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data),
            });
        }
        else {
            response = await fetch("values/InsertTaxonomicRankAsync", {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data),
            });
        }

        response.json().then(data => {
            console.dir(data);
            this.state.resetSelectedTR();
        });

    }

    render() {
        return (
            <div>
                <form onSubmit={(e) => this.handleSubmit(e)}>
                    <div className="mb-3">
                        <label htmlFor="TaxonomicRankID" className="form-label">ID</label>
                        <input type="number" className="form-control" id="TaxonomicRankID" name="TaxonomicRankID" aria-describedby="TaxonomicRankIDHelp" value={this.state.TaxonomicRankID} disabled readOnly />
                        <div id="TaxonomicRankIDHelp" className="form-text">ID</div>
                    </div>
                    <div className="mb-3">
                        <label htmlFor="parentTaxonomicRankID" className="form-label">Parent's ID</label>
                        <TaxonomicRankDropdown value={this.state.parentTaxonomicRankID} id="parentTaxonomicRankID" name="parentTaxonomicRankID" onChange={(e) => this.handleChange(e)} aria-describedby="parentTaxonomicRankIDHelp" />
                        <div id="parentTaxonomicRankIDHelp" className="form-text">Parent's ID</div>
                    </div>
                    <div className="mb-3">
                        <label htmlFor="taxonomicRankTypeId" className="form-label">Rank type</label>
                        <TaxonomicRankTypeDropdown value={this.state.taxonomicRankTypeId} id="taxonomicRankTypeId" name="taxonomicRankTypeId" onChange={(e) => this.handleChange(e)} aria-describedby="taxonomicRankTypeIdHelp" />
                        <div id="taxonomicRankTypeIdHelp" className="form-text">taxonomicRankType</div>
                    </div>
                    <div className="mb-3">
                        <label htmlFor="=name" className="form-label">Name</label>
                        <input type="string" className="form-control" id="name" name="name" value={this.state.name} onChange={(e) => this.handleChange(e)} maxLength="256" required />
                    </div>
                    <div className="mb-3">
                        <button type="submit" className="btn btn-primary">Save</button>
                        <button type="button" className="btn btn-secondary" onClick={() => this.state.resetSelectedTR()}>Return</button>
                    </div>
                </form>
            </div>
        );
    }

}
