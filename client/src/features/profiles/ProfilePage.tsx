import ProfileContent from './ProfileContent'
import { useProfile } from '../../lib/hooks/useProfile';
import { Grid2, Typography } from '@mui/material';
import { useParams } from 'react-router';
import ProfileHeader from './ProfileHeader';

export default function ProfilePage() {
  const { id } = useParams();
  const { profile, loadingProfile } = useProfile(id);

  if (loadingProfile) return <Typography>Loading profile...</Typography>
  if (!profile) return <Typography>Profile not found</Typography>
  return (
    <Grid2 container>
      <Grid2 size={12}>
        <ProfileHeader />
        <ProfileContent />
      </Grid2>
    </Grid2>
  )
}
