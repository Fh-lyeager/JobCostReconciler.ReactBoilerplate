import React, { Component } from 'react';

export class JobCostSummary extends Component {
    static displayName = JobCostSummary.name;

    constructor(props) {
        super(props);
        this.state = { jobSummary: [], loading: true };
        this.getJobSummary = this.getJobSummary.bind(this);
    }

    getJobSummary() {
        fetch('api/Job/JobTotals')
            .then(response => response.json())
            .then(data => {
                this.setState({ jobSummary: data, loading: false });
            });
    }

    static renderJobSummary(jobSummary) {
        return (
            <div>
                <h3>Job Cost Summary</h3>
                <table className='table table-striped'>
                    <thead>
                        <tr>
                            <th>JobNumber</th>
                            <th>Sapphire Total</th>
                            <th>Pervasive Total</th>
                            <th>TotalsMatch</th>
                        </tr>
                    </thead>
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
                </table>
            </div>
        );
    }

    render() {
        let contents = this.state.loading
            ? ""/*<p><em>Loading...</em></p>*/
            : JobCostSummary.renderJobSummary(this.state.jobSummary);

        let jobCostSummary = this.state.loading ? "" : JobCostSummary.renderJobSummary(this.state.jobSummary);

        return (
            <div>

                <div>
                    <h1>Job Cost Summary</h1>
                </div>

                <div>
                    <input type="text" name="jobnumber" placeholder="Job Number" /> &nbsp;
                    <button className="btn btn-primary" onClick={this.getJobSummary}>Get Job Details</button>
                </div>

                <div>
                    {jobCostSummary}
                    
                </div>
            </div>
        );
    }
}
