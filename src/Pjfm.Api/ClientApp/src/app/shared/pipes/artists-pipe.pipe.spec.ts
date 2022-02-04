import { ArtistsPipe } from './artists-pipe.pipe';

describe('ArtistsPipePipe', () => {
  it('create an instance', () => {
    const pipe = new ArtistsPipe();

    expect(pipe).toBeTruthy();
  });
});
