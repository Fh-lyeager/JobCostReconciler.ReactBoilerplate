import React, { Component } from 'react';

export class Reconciler extends Component {
    static displayName = Reconciler.name;

    constructor(props) {
        super(props);
        this.state = { jobSummary: [], loading: true };
        this.getJobSummary = this.getJobSummary.bind(this);
    }


    getJobSummary() {
        var jobNumber = this.textjobnumber;

        fetch('api/Job/JobTotals/'+jobNumber)
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
            : Reconciler.renderJobSummary(this.state.jobSummary);

        let jobCostSummary = this.state.loading ? "" : Reconciler.renderJobSummary(this.state.jobSummary);

        return (
            <div>

                <div>
                    <h1>Job Cost Reconciler</h1>
                </div>

                <div>
                    <label>
                        <input type="text" name="textjobnumber" /> &nbsp;
                        <button className="btn btn-primary" onClick={this.getJobSummary}>Get Job Details</button>
                    </label>
                </div>

                <div>
                    {jobCostSummary}
                    
                </div>
            </div>
        );
    }
}
