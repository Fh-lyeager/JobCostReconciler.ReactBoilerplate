const localStorageMock = {
  getItem: jest.fn(),
  setItem: jest.fn(),
  removeItem: jest.fn(),
  clear: jest.fn()
};
global.localStorage = localStorageMock;

const matchMediaMock = jest.fn(() => ({
  matches: false,
  addListener: jest.fn(),
  removeListener: jest.fn()
}));
global.window.matchMedia = matchMediaMock;
