import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import { Container, MenuItem } from '@mui/material';
import { Group } from '@mui/icons-material';

type Props ={
    openForm: () => void;
}

export default function NavBar({openForm}: Props) {
  return (
      <Box sx={{ flexGrow: 1 }}>
          <AppBar position="fixed"
              sx={{
                  backgroundImage: 'linear-gradient(135deg, #182a73 0%, #218aae 69%, #20a7ac 89%)'
              }}>
              <Container maxWidth="xl">
                  <Toolbar sx={{ display: 'flex', justifyContent: 'space-between' }}>
                      <Box>
                          <MenuItem sx={{ display: 'flex', gap: 2 }}>
                              <Group fontSize='large' />
                              <Typography variant="h4" fontWeight='bold'>Reactivities</Typography>
                          </MenuItem>
                      </Box>
                      <Box sx={{display: 'flex'}}>
                          <MenuItem sx={{fontSize: '1.2rem', textTransform: 'uppercase', fontWeight: 'bold'}}>Activities</MenuItem>
                          <MenuItem sx={{fontSize: '1.2rem', textTransform: 'uppercase', fontWeight: 'bold'}}>About</MenuItem>
                          <MenuItem sx={{fontSize: '1.2rem', textTransform: 'uppercase', fontWeight: 'bold'}}>Contact</MenuItem>
                      </Box>
                      <Button size='large' variant='contained' color='warning' onClick={openForm}>Create Activity</Button>
                  </Toolbar>
              </Container> 
          </AppBar>
      </Box>
  );
}
