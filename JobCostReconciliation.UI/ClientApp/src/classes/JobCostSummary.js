import React, {Component} from "react";

export default class JobCostSummary
{
    constructor(props) {
        this.state = { jobSummary: [] };
    }

    render() {
        fetch('api/Job/JobTotals')
            .then(response => response.json())
            .then(data => {
                this.setState({ jobSummary: data });
            });

        return this.state.jobSummary.toArray();
    }
}
