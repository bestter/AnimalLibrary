import React, { Component } from 'react';




export class UpdateTaxonomicRank extends Component {

    constructor(props) {
        super(props);
        this.state = {
            TaxonomicRankID: this.props.TaxonomicRank.TaxonomicRankID,
            parentTaxonomicRankID: this.props.TaxonomicRank.parentTaxonomicRankID,
            name: this.props.TaxonomicRank.name,
            nameFr: this.props.TaxonomicRank.nameFr,
            loading: true,
            resetSelectedTR: this.props.resetSelectedTR,
            newElement: this.props.newElement
        };
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    
    async handleSubmit(event) {
        event.preventDefault();
        
        const data = { TaxonomicRankID: this.state.TaxonomicRankID, ParentTaxonomicRankID: this.state.parentTaxonomicRankID, name: this.state.name, TaxonomicRankTypeID: this.state.TaxonomicRankTypeID };
        var response;
        if (this.state.newElement == false) {
            response = await fetch("values/UpdateTaxonomicRank", {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data),
            });
        }
        else {
            response = await fetch("values/InsertTaxonomicRank", {
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
                        <input type="number" className="form-control" id="parentTaxonomicRankID" name="parentTaxonomicRankID" value={this.state.parentTaxonomicRankID ?? -1} onChange={(e) => this.handleChange(e)} aria-describedby="parentTaxonomicRankIDHelp" />
                        <div id="parentTaxonomicRankIDHelp" className="form-text">Parent's ID</div>
                    </div>
                    <div className="mb-3">
                        <label htmlFor="taxonomicRankTypeID" className="form-label">Rank type</label>
                        {/*<input type="number" className="form-control" id="taxonomicRankTypeID" name="taxonomicRankTypeID" value={this.state.taxonomicRankTypeID ?? -1} onChange={(e) => this.handleChange(e)} aria-describedby="taxonomicRankTypeIDHelp" />*/}
                        <select value={this.state.taxonomicRankTypeID ?? -1} onChange={(e) => this.handleChange(e)}>
                            <option value="grapefruit">Pamplemousse</option>
                            <option value="lime">Citron vert</option>
                            <option selected value="coconut">Noix de coco</option>
                            <option value="mango">Mangue</option>
                        </select>
                        <div id="taxonomicRankTypeIDHelp" className="form-text">taxonomicRankTypeID</div>
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

export class ReadTaxonomicRank extends Component {

    constructor(props) {
        super(props);
        this.state = { TaxonomicRanks: [], loading: true, selectedTR: null, newTr: false, emptyTr: null };
    }

    componentDidMount() {
        this.populateTaxonomicRank();
    }

    edit(TaxonomicRank) {
        this.setState({ selectedTR: TaxonomicRank });
    }

    resetSelectedTR() {
        this.setState({ selectedTR: null });
        console.log('resetSelectedTR');
    }

    createElement() {
        this.setState({ newTr: true });        
    }


    getCode() {
        if (this.state.newTr == true) {
            return <UpdateTaxonomicRank TaxonomicRank={this.state.emptyTr} resetSelectedTR={() => this.resetSelectedTR()} newElement={true} />;
        }
        else if (this.state.selectedTR == null) {
            return (
                <div>
                    <ul className="list-group">
                        {
                            this.state.TaxonomicRanks.map((item, key) => {
                                return <li className="list-group-item" key={item.TaxonomicRankID}> {item.name}<button onClick={() => { this.edit(item); }} className="btn btn-primary bi bi-pencil">edit</button></li>
                            })
                        }
                    </ul>                    
                </div>
            );
        }
        else {
            return <UpdateTaxonomicRank TaxonomicRank={this.state.selectedTRua} resetSelectedTR={() => this.resetSelectedTR()} newElement={ false } />;
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
    async populateTaxonomicRank() {
        const response = await fetch('values/GetAllTaxonomicRank');
        const data = await response.json();

        const response2 = await fetch('values/GetEmptyTaxonomicRank');
        const data2 = await response2.json();

        this.setState({ TaxonomicRanks: data, loading: false, emptyTr: data2 });
    }
}


export class TaxonomicRank extends Component {

    render() {
        return (            
                <ReadTaxonomicRank />
            );
    }
}