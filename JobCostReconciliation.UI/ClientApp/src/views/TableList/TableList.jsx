import React from 'react';
// @material-ui/core components
import withStyles from "@material-ui/core/styles/withStyles";
// core components
import GridItem from "components/Grid/GridItem.jsx";
import GridContainer from "components/Grid/GridContainer.jsx";
import Table from "components/Table/Table.jsx";
import Card from "components/Card/Card.jsx";
import CardHeader from "components/Card/CardHeader.jsx";
import CardBody from "components/Card/CardBody.jsx";

import JobCostSummary from "functions/JobCostSummary.jsx";

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

function TableList(props) {
    const { classes } = props;
    const { jobSummary } = JobCostSummary;

    return (
        <GridContainer>
            <GridItem xs={12} sm={12} md={12}>
                <Card>
                    <CardHeader color="primary">
                        <h4 className={classes.cardTitleWhite}>Simple Table</h4>
                        <p className={classes.cardCategoryWhite}>
                            Here is a subtitle for this table
                        </p>
                    </CardHeader>
                    <CardBody>
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
                    </CardBody>
                </Card>
            </GridItem>
        </GridContainer>
    );
}

export default withStyles(styles)(TableList);
