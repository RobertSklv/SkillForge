import { DropdownItem } from '@components/dropdown-item/DropdownItem';
import { Dropdown } from '@components/dropdown/Dropdown';
import { Icon } from '@components/icon/Icon';
import { NavLink } from '@components/nav-link/NavLink';
import { Navbar } from '@components/navbar/Navbar';
import { useCurrentUser } from 'hooks/useCurrentUser';
import { useNavigate } from 'react-router-dom';
import { deleteAuthToken } from 'skillforge-common/auth';
import { getImagePath } from 'skillforge-common/util';

export function Header() {
    const { currentUser, logoutUser } = useCurrentUser();
    const navigate = useNavigate();

    async function logout() {
        logoutUser();
        deleteAuthToken();

        navigate('/join');
    }

    function renderLogo() {
        return (
            <div className="d-flex justify-content-center align-items-center">
                <img src="src/assets/logo.png" className="me-1" style={{ height: '22px' }} alt="Logo" />
                <span className="d-none d-md-block">SkillForge</span>
            </div>
        );
    }

    function renderLinks() {
        return (
            <>
                {currentUser ? (
                    <>
                        <NavLink href="/article/create">Create Article</NavLink>
                        <Dropdown isNav={true} buttonSnippet={(
                            <>
                                <span>{currentUser.Name}</span>
                                <img src={getImagePath(currentUser.AvatarPath)} width="20" height="20" className="rounded-circle border-1 ms-1 object-fit-cover" alt="Your avatar" />
                            </>
                        )}>
                            <DropdownItem href={`/user/${currentUser.Name}`}>Account</DropdownItem>
                            <DropdownItem href="/account">Edit Account</DropdownItem>
                            <DropdownItem href="/article/create">Create Article</DropdownItem>
                            <DropdownItem type="button" onClick={logout}>Logout</DropdownItem>
                        </Dropdown>
                    </>
                ) : (
                    <NavLink href="/join">
                        <span>Join us</span>
                        <Icon type="person-circle" />
                    </NavLink>
                )}
            </>
        );
    }

    return (
        <header>
            <Navbar logoLink="/" logoSnippet={renderLogo()} linksSnippet={renderLinks}>
                {/* {#if page.url.pathname !== '/search'}
                    <HeaderSearchBar />
                {/if} */}
            </Navbar>
        </header>
    );
}
