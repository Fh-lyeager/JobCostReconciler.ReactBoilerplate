import React from "react";
import Navigation from "../../components/Navigation/Navigation";

export default () => <Navigation navItems={navItems} />;

const navItems = [
  {
    abbreviation: "FH",
    name: "Fischer Homes",
    links: [
      {
        href: "/",
        name: "Home",
        icon: "home",
        private: false
      },
      {
        href: "/not-found",
        name: "Not Found",
        icon: "ban",
        private: false
      }
    ]
  },
  {
    abbreviation: "AO",
    name: "Another one",
    links: [
      {
        href: "hi",
        name: "Hi"
      },
      {
        href: "no",
        name: "No"
      }
    ]
  },
  {
    abbreviation: "HIO",
    name: "Hio",
    links: [
      {
        href: "three",
        name: "Times"
      },
      {
        href: "two",
        name: "Another long one this time"
      }
    ]
  }
];
