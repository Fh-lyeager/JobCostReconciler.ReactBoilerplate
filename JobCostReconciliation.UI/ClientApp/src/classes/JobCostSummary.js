import React, {Component} from "react";

export default class JobCostSummary extends Component
{
    constructor(props) {
        super(props);
        this.state = { jobSummary: [], loading: true };
    }

    render() {
        fetch('api/Job/JobTotals')
            .then(response => response.json())
            .then(data => {
                this.setState({ jobSummary: data, loading: false });
            });

        return this.state.jobSummary.toArray();
    }
}
