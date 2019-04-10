import React, { Component } from 'react';
import withStyles from "@material-ui/core/styles/withStyles";
// core components
import GridItem from "components/Grid/GridItem.jsx";
import GridContainer from "components/Grid/GridContainer.jsx";
import Table from "components/Table/Table.jsx";
import Card from "components/Card/Card.jsx";
import CardHeader from "components/Card/CardHeader.jsx";
import CardBody from "components/Card/CardBody.jsx";

const styles = {
    cardCategoryWhite: {
        "&,& a,& a:hover,& a:focus": {
            color: "rgba(255,255,255,.62)",
            margin: "0",
            fontSize: "14px",
            marginTop: "0",
            marginBottom: "0"
        },
        "& a,& a:hover,& a:focus": {
            color: "#FFFFFF"
        }
    },
    cardTitleWhite: {
        color: "#FFFFFF",
        marginTop: "0px",
        minHeight: "auto",
        fontWeight: "300",
        fontFamily: "'Roboto', 'Helvetica', 'Arial', sans-serif",
        marginBottom: "3px",
        textDecoration: "none",
        "& small": {
            color: "#777",
            fontSize: "65%",
            fontWeight: "400",
            lineHeight: "1"
        }
    }
};

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
            <GridContainer>
                <GridItem xs={12} sm={12} md={12}>
                    <Card>
                        <CardHeader color="primary">
                            <h4 className={classes.cardTitleWhite}>Simple Table</h4>
                        </CardHeader>
                        <CardBody>
                            <Table
                                tableHeaderColor="primary"
                                tableHead={["Job Number", "Sapphire Total", "Pervasive Total", ""]}
                                tableData={this.state.jobSummary.map(job =>
                                    <tr key={job.jobNumber}>
                                        <td>{job.jobNumber}</td>
                                        <td>{job.sapphireEgmTotal}</td>
                                        <td>{job.pervasiveEgmTotal}</td>
                                        <td>{job.totalsMatch}</td>
                                    </tr>
                                )}
                            />
                        </CardBody>
                    </Card>
                </GridItem>
            </GridContainer>
        );
    }

    render() {
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
