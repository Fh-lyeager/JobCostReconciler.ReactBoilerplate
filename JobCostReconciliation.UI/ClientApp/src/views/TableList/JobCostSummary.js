import React, { Component } from 'react';
import PropTypes from "prop-types";
import Button from 'components/CustomButtons/Button.jsx';

import GridItem from "components/Grid/GridItem.jsx";
import GridContainer from "components/Grid/GridContainer.jsx";
import Table from "components/Table/Table.jsx";
import Card from "components/Card/Card.jsx";
import CardHeader from "components/Card/CardHeader.jsx";
import CardBody from "components/Card/CardBody.jsx";

class JobCostSummary extends React.Component {
    constructor(props) {
        super(props);
        this.state = { jobNumber: '', jobSummary: [], loading: true };
        this.getJobSummary = this.getJobSummary.bind(this);
        this.updateInputValue = this.updateInputValue.bind(this);
    }

    getJobSummary(jobNumber) {
        fetch('api/Job/JobTotals')
            .then(response => response.json())
            .then(data => {
                this.setState({ jobSummary: Array.from(data), loading: false });
            });
    }

    static renderJobSummary(jobSummary) {
        return (
                    <tbody>
                        {jobSummary.map(job =>
                            <tr key={job.jobNumber}>
                                <td>{job.jobNumber}</td>
                                <td>{job.sapphireEgmTotal}</td>
                                <td>{job.pervasiveEgmTotal}</td>
                                <td>{job.totalsMatch}</td>
                            </tr>
                        )}
                    </tbody>
        );
    }

    updateInputValue(evt) {
        this.setState({
            inputValue: evt.target.value
        });
    }

    render() {
        let jobTotals = this.state.loading ? "" : JobCostSummary.renderJobSummary(this.state.jobSummary);

        return (
            <div>
                <div>
                    <h1>Job Cost Reconciler</h1>
                    <p>I am here: {this.state.jobNumber}</p>
                </div>
                <div>
                    <label>
                        <input value={this.state.inputValue} onChange={evt => this.updateInputValue(evt)} /> &nbsp;
                        <Button
                            className="Button"
                            color="primary"
                            onClick={() => {
                                this.getJobSummary(this.state.inputValue);
                            }}
                            textDecoration="Get Job Details">
                            Get Job Details
                        </Button>
                    </label>
                </div>
                <GridContainer>
                    <GridItem xs={12} sm={12} md={12}>
                        <Card>
                            <CardHeader color="primary">
                                <h4>Job Cost Summary</h4>
                            </CardHeader>
                            <CardBody>
                                <table width="100%">
                                    <thead>
                                        <tr width="100%">
                                            <th>JobNumber</th>
                                            <th>Sapphire Total</th>
                                            <th>Pervasive Total</th>
                                            <th>TotalsMatch</th>
                                        </tr>
                                    </thead>
                                    {jobTotals}
                                </table>
                            </CardBody>
                        </Card>
                    </GridItem>
                </GridContainer>
            </div>
        );
    }
}

export default (JobCostSummary);