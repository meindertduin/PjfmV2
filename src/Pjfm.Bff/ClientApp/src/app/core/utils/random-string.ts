export const generateRandomString = (length: number): string => {
  const characters = 'abcdefghijklmnopqrstuvwxyz1234567890';
  let randomString = '';

  for (let i = 0; i < length; i++) {
    const randomIndex = Math.floor(Math.random() * characters.length);
    randomString += characters[randomIndex];
  }

  return randomString;
};
