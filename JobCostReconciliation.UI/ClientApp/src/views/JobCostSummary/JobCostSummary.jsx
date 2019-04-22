import React, { Component } from 'react';
import PropTypes from "prop-types";
import Button from 'components/CustomButtons/Button.jsx';

import GridItem from "components/Grid/GridItem.jsx";
import GridContainer from "components/Grid/GridContainer.jsx";
import Table from "components/Table/Table.jsx";
import TableHead from "@material-ui/core/TableHead";
import TableBody from "@material-ui/core/TableBody";
import TableRow from "@material-ui/core/TableRow";
import TableRowColumn from "@material-ui/core/TableRow";
import TableCell from "@material-ui/core/TableCell";
import Card from "components/Card/Card.jsx";
import CardHeader from "components/Card/CardHeader.jsx";
import CardBody from "components/Card/CardBody.jsx";

class JobCostSummaryPage extends React.Component {
    constructor(props, classes) {
        super(props, classes);
        this.state = { jobTotals: [], loadingJobTotals: true, jobSummary: [], loadingJobSummary: true };
        this.getJobSummary = this.getJobSummary.bind(this);
        this.getJobSummaryByJob = this.getJobSummaryByJob.bind(this);
    }

    getJobSummary() {
        this.setState({loadingJobSummary: true});

        fetch('api/JobCost/WorkflowJobsEgmTotals')
            .then(response => response)
            .then(data => {
                this.setState({ jobSummary: Array.from(data), loadingJobSummary: false });
            });
    }

    getJobSummaryByJob(jobNumber) {
        this.setState({ loadingJobTotals: true });

        fetch('api/JobCost/JobTotals/'.concat('APT015810000'))
            .then(response => response.json())
            .then(data => {
                this.setState({ jobTotals: data, loadingJobTotals: false });
            });
    }

    static renderJobCostSummary(jobSummary) {
        return (
                <CardBody>
                    <table>
                        <thead>
                            <tr>
                                <th>JobNumber</th>
                                <th>Sapphire Total</th>
                                <th>Pervasive Total</th>
                            </tr>
                        </thead>
                            <tbody>
                                {jobSummary.map(job =>
                                    <tr key={job.jobNumber}>
                                        <td>{job.jobNumber}</td>
                                        <td>{job.sapphireEgmTotal}</td>
                                        <td>{job.pervasiveEgmTotal}</td>
                                    </tr>
                                )}
                        </tbody>
                    </table>
                </CardBody>
        )
    }

    static renderJobTotals(jobTotals, classes) {
        return (
            <Card>
                <CardHeader color="primary">
                    <h4 className={classes.cardTitleWhite}>Job Cost Summary By Job</h4>
                </CardHeader>
                <CardBody>
                    <Table 
                        
                    />
                </CardBody>
            </Card>
        )
    }

    render() {

        let jobCostSummary = this.state.jobSummary.length > 0 ? JobCostSummaryPage.renderJobCostSummary(this.state.jobSummary, this.props) : [];
        let jobTotalsByJob = this.state.jobTotals != undefined && this.state.jobTotals != [] ? JobCostSummaryPage.renderJobTotals(this.state.jobTotals, this.props) : [];

        let classes = this.props;
        

        return (
            <div>
                <input ref="jobNumberInput" type="text" placeholder="Job Number" />
                <Button
                    className="Button"
                    color="info"
                    onClick={() => {
                        this.getJobSummaryByJob(this.refs.jobNumberInput.value);
                    }}>
                    Get Job Cost Summary
                    </Button>
                <GridContainer>
                    <GridItem xs={12} sm={12} md={12}>
                        {jobTotalsByJob}
                    </GridItem>
                    <GridItem xs={12} sm={12} md={12}>
                        <Button
                            className="Button"
                            color="info"
                            onClick={() => {
                                this.getJobSummary();
                            }}>
                            Get Job Cost Summary
                        </Button>
                            <Card>
                                <CardHeader color="primary">
                                    <h4 className={classes.cardTitleWhite}>Job Cost Summary</h4>
                                </CardHeader>
                            {jobCostSummary}
                        </Card>
                    </GridItem>
                </GridContainer>
                </div>
        );
    }
}

export default (JobCostSummaryPage);