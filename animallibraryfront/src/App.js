import React, { Component } from 'react';
import {
    BrowserRouter as Router,
    Routes,
    Route,
    Link
} from 'react-router-dom';
import Navbar from './components/Navbar';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import { TaxonomicRankType } from './components/TaxonomicRankType';
import { TaxonomicRank } from './components/TaxonomicRank';
import { Species } from './components/Species';

export default class App extends Component {
    static displayName = App.name;

    constructor(props) {
        super(props);
        this.state = { forecasts: [], loading: true, hasError: false };
    }

    static getDerivedStateFromError(error) {
        // Update state so the next render will show the fallback UI.
        return { hasError: true };
    }

    componentDidMount() {
        this.populateWeatherData();
    }

    static renderForecastsTable(forecasts) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Temp. (C)</th>
                        <th>Temp. (F)</th>
                        <th>Summary</th>
                    </tr>
                </thead>
                <tbody>
                    {forecasts.map(forecast =>
                        <tr key={forecast.date}>
                            <td>{forecast.date}</td>
                            <td>{forecast.temperatureC}</td>
                            <td>{forecast.temperatureF}</td>
                            <td>{forecast.summary}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    static renderMenu() {

        return (           
            <Router>
                <Routes>                    
                    <Route path='/' element={<Home />} />
                    <Route path='/counter' element={ <Counter />} />
                    <Route path='/fetch-data' element={<FetchData />} />
                    <Route path='/taxonomicRankType' element={ <TaxonomicRankType />} />
                    <Route path='/taxonomicRank' element={ <TaxonomicRank />} />
                    <Route path='/species' element={ <Species />} />
                </Routes>     
            </Router>
            
            );
    }

    getContents() {                   
        return (<div><span>test</span></div>);
        
    }

    render() {
        //const Contents = App.renderMenu();

        return (
            <div>
                <h1 id="tabelLabel">Animal library</h1>
                <p>This component demonstrates fetching data from the server.</p>                
                {App.renderMenu()}
                <Navbar />
            </div>
        );
    }

    async populateWeatherData() {
        const response = await fetch('weatherforecast');
        const data = await response.json();
        this.setState({ forecasts: data, loading: false });
    }
}
