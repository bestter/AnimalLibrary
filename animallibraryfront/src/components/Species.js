import React, { Component } from 'react';
import { SpeciesList } from './SpeciesList';
import { Specie } from './Specie';

export class Species extends Component {

    constructor(props) {
        super(props);
        //this.state = { emptySpecie: null, species: [] };
        this.state = { loading: true, selectedSpecie: null, newSpecie: false, emptySpecie: this.props.EmptySpecie, species: [] };

    }

    async componentDidMount() {
        
        await fetch('values/GetSpeciesAsync')
            .then(resp => resp.json())
            .then(data => this.setState({ species: data }))
            .then(
                fetch('values/GetEmptySpecie')
                    .then(resp => resp.json())
                    .then(data2 => this.setState({ emptySpecie: data2, selectedSpecie: data2, loading: false })));
    }

    createElement() {
        this.setState({ newSpecie: true, selectedSpecie: this.state.emptySpecie });
    }

    render() {
        var content;
        if (this.state.newSpecie === false) {
            content = <SpeciesList Species={this.state.species} />;
        }
        else {
            content = <Specie Specie={this.state.emptySpecie} />;
        }
        return (
            <div>
                <h1>Species</h1>

                <div className="mb-2">
                    {content}
                </div>
                <div className="mb-2">
                    <button type="button" className="btn btn-success" onClick={() => this.createElement()}>New</button>
                </div>
            </div>
        );
    }
}


export class SpeciesStuff extends Component {

    constructor(props) {
        super(props);
        this.state = { loading: true, selectedSpecie: null, newSpecie: false, emptySpecie: this.props.EmptySpecie, species: this.props.Species };
    }

    createElement() {
        this.setState({ newSpecie: true, selectedSpecie: this.state.emptySpecie });
    }

    render() {        
        var content;
        if (this.state.newSpecie === false) {
            content = <SpeciesList Species = { this.state.species } />;
        }
        else {
            content = <Specie Specie={this.state.emptySpecie} />;
        }
        return (
            <div>
                <h1>Species</h1>

                <div className="mb-2">
                    {content}
                </div>
                <div className="mb-2">
                    <button type="button" className="btn btn-success" onClick={() => this.createElement()}>New</button>
                </div>
            </div>
        );
    }
}