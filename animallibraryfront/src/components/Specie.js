import React, { Component } from 'react';
import { TaxonomicRankDropdown } from './TaxonomicRankDropdown';

export class Specie extends Component {

    constructor(props) {
        super(props);

        if (this.props.Specie) {

            this.state = { id: this.props.Specie.id ?? "", name: this.props.Specie.name ?? "", latinName: this.props.Specie.latinName ?? "", description: this.props.Specie.description ?? "", ParentTaxonomicRankID: this.props.Specie.ParentTaxonomicRankID, newSpecie: false };
        }
        else {
            this.state = { id: 0, name: "", latinName: "", description: "", ParentTaxonomicRankID: -1, newSpecie: false };
        }
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    async handleSubmit(event) {
        event.preventDefault();
        const data = { Id: this.state.id, ParentTaxonomicRank: this.state.ParentTaxonomicRankID, name: this.state.name, latinName: this.state.latinName, Description: this.state.description };
        var response;
        if (this.state.newSpecie === false) {
            response = await fetch("values/UpdateSpecieAsync", {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data),
            });
        }
        else {
            response = await fetch("values/InsertSpecieAsync", {
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
            <form>
                    <div className="jumbotron">
                    <h1 className="display-4">{ this.state.name }</h1>
                    </div>
                <div className="mb-3 form-group">
                    <label htmlFor="exampleTaxonomy">Taxonomy</label>
                        <input type="text" className="form-control" id="name" value={this.state.name} lang="en-CA" aria-describedby="taxonomyHelp" placeholder="Enter name" onChange={(e) => this.handleChange(e)} />
                    <div id="taxonomyHelp" className="form-text">Name of the specie</div>
                </div>
                <div className="mb-3 form-group">
                        <label htmlFor="exampleLatinName" lang="la">Latin name</label>
                        <input type="text" className="form-control" id="latinName" value={this.state.latinName} placeholder="Enter latin name" onChange={(e) => this.handleChange(e)}  />
                    </div>          
                    <div className="mb-3 form-group">
                        <label htmlFor="exampleDescription">Description</label>
                        <input type="text" className="form-control" id="description" lang="en-CA" value={this.state.description} placeholder="Enter description" onChange={(e) => this.handleChange(e)} />
                    </div>                 
                <div className="mb-3 form-group">
                    <label htmlFor="parentTaxonomicRankID" className="form-label">Parent's ID</label>
                        <TaxonomicRankDropdown value={this.state.ParentTaxonomicRankID} id="parentTaxonomicRankID" name="parentTaxonomicRankID" onChange={(e) => this.handleChange(e)} aria-describedby="parentTaxonomicRankIDHelp" />
                    <div id="parentTaxonomicRankIDHelp" className="form-text">Parent's ID</div>
                </div>
                <div className="mb-3"/>
                <div className="mb-3 form-group">
                    <button type="submit" className="btn btn-primary">Save</button>
                    <button type="button" className="btn btn-secondary">Return</button>
                </div>
            </form>
        </div>
        );
    }
}