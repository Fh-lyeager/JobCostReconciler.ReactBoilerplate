import React, { Component } from 'react';
//import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { NavLink } from 'reactstrap';
import { Collapse, Container, NavbarBrand, NavItem } from 'react-bootstrap';
import { NavDropdown, Nav, Button, Form, FormControl, Navbar, ReactBootstrapStyle } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
//import './HeaderSearch.css';

export class NavMenu extends Component {
    static displayName = NavMenu.name;

    constructor(props) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true
        };
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }



    render() {
        return (
            <header>
                <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                    <Container>

                        <Navbar.Brand href="/">
                            <img
                                src="FischerLogo cropped.jpg"
                                width="50"
                                //height="60"
                                className="d-inline-block align-top"
                                alt="Fischer Homes"
                            />
                        </Navbar.Brand>
                        <div>
                            <input type="text" placeholder="Job Number" /> &nbsp;
                        <button className="btn btn-primary" onClick={this.getJobSummary}>Get Job Details</button>
                        </div>
                        <Navbar.Toggle onClick={this.toggleNavbar} className="mr-2" />
                        <NavDropdown title="Search">
                            <NavDropdown.Item><input type="text" placeholder="Job Number"/> </NavDropdown.Item>
                        </NavDropdown>
                        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
                            <ul className="navbar-nav flex-grow">
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/">Reconcile</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/JobCostSummary">Job Cost</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/">Audit</NavLink>
                                </NavItem>
                            </ul>

                        </Collapse>
                    </Container>
                </Navbar>

            </header>



        );
    }
}
