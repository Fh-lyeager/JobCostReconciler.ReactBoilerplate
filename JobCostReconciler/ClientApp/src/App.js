import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Reconciler } from './components/Reconciler';
import { JobCostSummary } from './components/JobCostSummary';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
            <Route exact path='/' component={Reconciler} />
            <Route exact path='/jobcostsummary' component={JobCostSummary} />
      </Layout>
    );
  }
}
