import React, { Component } from 'react';


export class UpdateTaxonomicRankType extends Component {

    constructor(props) {
        super(props);
        this.state = {
            taxonomicRankTypeID: this.props.taxonomicRankType.taxonomicRankTypeID,
            parentTaxonomicRankTypeID: this.props.taxonomicRankType.parentTaxonomicRankTypeID,
            name: this.props.taxonomicRankType.name,
            nameFr: this.props.taxonomicRankType.name,
            loading: true
        };
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    async handleSubmit(event) {
        event.preventDefault();
        
        const data = { taxonomicRankTypeID: this.state.taxonomicRankTypeID, parentTaxonomicRankTypeID: this.state.parentTaxonomicRankTypeID, name: this.state.name, nameFr: this.state.nameFr };
        
        const response = await fetch("values/UpdateTaxonomicRankType", {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data),
        });

        response.json().then(data => {
            console.dir(data);
        });

    }

    render() {
        return (
            <div>
                <form onSubmit={(e) => this.handleSubmit(e)}>
                    <div className="mb-3">
                        <label htmlFor="taxonomicRankTypeID" className="form-label">ID</label>
                        <input type="number" className="form-control" id="taxonomicRankTypeID" name="taxonomicRankTypeID" aria-describedby="taxonomicRankTypeIDHelp" value={this.state.taxonomicRankTypeID} disabled readOnly />
                        <div id="taxonomicRankTypeIDHelp" className="form-text">ID</div>
                    </div>
                    <div className="mb-3">
                        <label htmlFor="parentTaxonomicRankTypeID" className="form-label">Parent's ID</label>
                        <input type="number" className="form-control" id="parentTaxonomicRankTypeID" name="parentTaxonomicRankTypeID" value={this.state.parentTaxonomicRankTypeID ?? -1} onChange={(e) => this.handleChange(e)} aria-describedby="parentTaxonomicRankTypeIDHelp" />
                        <div id="parentTaxonomicRankTypeIDHelp" className="form-text">Parent's ID</div>
                    </div>
                    <div className="mb-3">
                        <label htmlFor="=name" className="form-label">Name</label>
                        <input type="string" className="form-control" id="name" name="name" value={this.state.name} onChange={(e) => this.handleChange(e)} maxLength="256" required />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="=nameFr" className="form-label">Name in french</label>
                        <input type="string" className="form-control" id="nameFr" name="nameFr" value={this.state.nameFr} onChange={(e) => this.handleChange(e)} maxLength="256" required />
                    </div>
                    <div className="mb-3">
                        <button type="submit" className="btn btn-primary">Save</button>
                    </div>
                </form>
            </div>
        );
    }

}

export class ReadTaxonomicRankType extends Component {

    constructor(props) {
        super(props);
        this.state = { taxonomicRankTypes: [], loading: true, selectedTRT: null };
    }

    componentDidMount() {
        this.populateTaxonomicRankType();
    }

    edit(taxonomicRankType) {
        this.setState({ selectedTRT: taxonomicRankType });
    }

    render() {
        if (this.state.selectedTRT == null) {
            return (
                <ul className="list-group">
                    {
                        this.state.taxonomicRankTypes.map((item, key) => {
                            return <li className="list-group-item" key={item.taxonomicRankTypeID}> {item.name}<button onClick={() => { this.edit(item); }} className="btn btn-primary bi bi-pencil">edit</button></li>
                        })
                    }
                </ul>
            );
        }
        else {
            return <UpdateTaxonomicRankType taxonomicRankType={this.state.selectedTRT} />;
        }
    }
    async populateTaxonomicRankType() {
        const response = await fetch('values/GetAll');
        const data = await response.json();
        this.setState({ taxonomicRankTypes: data, loading: false });
    }
}


export class TaxonomicRankType extends Component {

    render() {
        return (
            <div>
                <ReadTaxonomicRankType />
            </div>);
    }
}