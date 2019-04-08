import React from "react";
import Profile from "./Profile";
import HeaderItem from "./HeaderItem";
import SubNavItem from "./SubNavItem";

import "./Navigation.css";

export default class Navigation extends React.Component {
  state = {
    selectedNav: this.props.navItems[0]
  };

  render() {
    return (
      <div className="navigation">
        <ul className="navigation__header-items">
          {this.props.navItems.map((item, key) => (
            <HeaderItem
              className="navigation__header-items__list__item"
              item={item}
              key={key}
              onClick={() => {
                this.setState({
                  selectedNav: item
                });
              }}
            />
          ))}
        </ul>
        <nav className="navigation__sub-nav-items navigation-vertical">
          <ul className="navigation__sub-nav-items__list">
            {this.state.selectedNav.links.map((link, key) => (
              <SubNavItem
                className="navigation__sub-nav-items__list__item"
                link={link}
                key={key}
              />
            ))}
          </ul>

          <Profile />
        </nav>
      </div>
    );
  }
}

Navigation.defaultProps = {
  navItems: []
};
