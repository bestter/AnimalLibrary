import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import { TaxonomicRankType } from './components/TaxonomicRankType';
import { TaxonomicRank } from './components/TaxonomicRank';
import { Species } from './components/Species';

import './custom.css'

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                    <Route exact path='/' component={Home} />
                    <Route path='/counter' component={Counter} />
                    <Route path='/fetch-data' component={FetchData} />
                    <Route path='/taxonomicRankType' component={TaxonomicRankType} />
                    <Route path='/taxonomicRank' component={TaxonomicRank} />
                    <Route path='/species' component={Species} />
                
            </Layout>
        );
    }
}
